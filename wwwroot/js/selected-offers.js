window.addEventListener('DOMContentLoaded', async () => {
  const params = new URLSearchParams(window.location.search);
  const salary = parseFloat(params.get('salary'));
  const amount = parseFloat(params.get('amount'));
  const duration = parseInt(params.get('duration'));

  try {
    const response = await fetch('http://localhost:5102/api/loan-comparison/get-matches', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ salary, amount, duration })
    });

    const container = document.getElementById('offers-list');
    container.innerHTML = '';

    if (!response.ok) {
      container.innerHTML = `<p>لا توجد عروض مناسبة لك حاليًا.</p>`;
      return;
    }

    const offers = await response.json();

    offers.forEach(offer => {
      const html = `
        <div class="offer-card">
          <h2>البنك: ${offer.bankName}</h2>
          <h4>نسبة الفائدة: ${offer.interestRate}%</h4>
          <p>المبلغ: ${offer.loanAmount} ريال</p>
          <p>المدة: ${offer.duration} أشهر</p>
          
          <p>القسط الشهري: ${offer.monthlyPayment} ريال</p>
        </div>
      `;
      container.innerHTML += html;
    });
  } catch (err) {
    console.error(err);
    document.getElementById('offers-list').innerHTML = '<p>حدث خطأ أثناء تحميل العروض.</p>';
  }
});
