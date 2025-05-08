document.addEventListener('DOMContentLoaded', async () => {
    if (!window.Square) {
        console.error('Square SDK failed to load.');
        return;
    }

    // Use dynamically injected values if available; fallback to hardcoded sandbox credentials.
    const appId = window.squareAppId || 'sandbox-sq0idb-WOtifQSKNybOUxarQkzTEg';
    const locationId = window.squareLocationId || 'LMZKPRF20WXFP';

    try {
        const payments = window.Square.payments(appId, locationId);
        const card = await payments.card();
        await card.attach('#card-container');

        // listen for the form submission that triggers payment
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
    console.log("token", token);
    try {
        const response = await fetch('/api/payment/process', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ token: token, amount: 1000 })
        });
        const data = await response.json();
        if (response.ok) {
            alert('Payment processed successfully!');
        } else {
            alert('Payment failed: ' + data.error);
        }
    } catch (error) {
        console.error('Error sending payment token to the server:', error);
    }
}