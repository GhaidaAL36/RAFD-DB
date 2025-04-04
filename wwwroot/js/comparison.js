document.getElementById('comparison-form').addEventListener('submit', function (e) {
  e.preventDefault();

  const salary = document.getElementById('salary').value;
  const amount = document.getElementById('requested-amount').value;
  const duration = document.getElementById('duration').value;

  const query = new URLSearchParams({
    salary,
    amount,
    duration
  });

  window.location.href = `selected-offers.html?${query.toString()}`;
});
