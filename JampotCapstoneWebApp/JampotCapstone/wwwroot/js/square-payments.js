async function getSquareConfig() {
    const response = await fetch('/api/config/square');
    if (!response.ok) {
        throw new Error('Failed to load Square configuration');
    }
    return response.json();
}

document.addEventListener('DOMContentLoaded', async () => {
    if (!window.Square) {
        console.error('Square SDK failed to load.');
        return;
    }

    const config = await getSquareConfig();
    const appId = config.applicationId;
    const locationId = config.locationId;

    try {
        const payments = window.Square.payments(appId, locationId);
        const card = await payments.card();
        await card.attach('#card-container');

        const paymentForm = document.getElementById('payment-form');
        if (paymentForm) {
            paymentForm.addEventListener('submit', async (event) => {
                event.preventDefault();
                const result = await card.tokenize();
                if (result.status === 'OK') {
                    console.log('Payment token obtained:', result.token);
                    processPayment(result.token);
                } else {
                    console.error('Tokenization error:', result);
                    alert('There was an error processing your payment. Please try again.');
                }
            });
        }
    } catch (error) {
        console.error('Error initializing Square payments:', error);
    }
});

async function processPayment(token) {
    //unit conversion from dollars to cents for payment.
    const cartSubtotal = parseInt(document.getElementById('cartSubtotal').value, 10) * 100;

    //phone necessary for loyalties
    const customerPhone = document.getElementById('customerPhone').value;

    console.log("cart total and loyalty number:", cartSubtotal, customerPhone);
    try {
        const response = await fetch('/api/payment/process', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                token: token,
                amount: cartSubtotal,
                //phone: customerPhone,
            })
        });
        const data = await response.json();
        if (response.ok) {
            const modalEl = document.getElementById('squarePaymentModal');
            const paymentModal = bootstrap.Modal.getOrCreateInstance(modalEl);
            paymentModal.hide();

            document.body.classList.remove('modal-open');
            const backdrop = document.querySelector('.modal-backdrop');
            if (backdrop) {
                backdrop.parentNode.removeChild(backdrop);
            }

            const successModalEl = document.getElementById('paymentSuccessModal');
            const successModal = new bootstrap.Modal(successModalEl);
            successModal.show();
        } else {
            alert('Payment failed: ' + data.error);
        }
    } catch (error) {
        console.error('Error sending payment token to the server:', error);
    }
}