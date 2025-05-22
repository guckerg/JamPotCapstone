document.addEventListener('DOMContentLoaded', async () => {
    if (!window.Square) {
        console.error('Square SDK failed to load.');
        return;
    }

    const appId = 'sandbox-sq0idb-WOtifQSKNybOUxarQkzTEg';
    const locationId = 'LMZKPRF20WXFP';

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
                    //console.log('Payment token obtained:', result.token);
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
    const cartSubtotal = parseInt(document.getElementById('cartSubtotal').value, 10) * 100; //unit conversion from dollars to cents for payment.
    try {
        const response = await fetch('/api/payment/process', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ token: token, amount: cartSubtotal })
        });
        const data = await response.json();
        if (response.ok) {
            alert('Payment processed successfully!');
            const modalEl = document.getElementById('squarePaymentModal');
            const paymentModal = bootstrap.Modal.getOrCreateInstance(modalEl);
            paymentModal.modal('hide');

            //document.body.classList.remove('modal-open');
            //const backdrop = document.querySelector('.modal-backdrop');
            //if (backdrop) {
            //    backdrop.parentNode.removeChild(backdrop);
            //}
        } else {
            alert('Payment failed: ' + data.error);
        }
    } catch (error) {
        console.error('Error sending payment token to the server:', error);
    }
}