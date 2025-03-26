//bank interface

fetch("/bank-dashboard/get-bank-data")
  .then((res) => res.json())
  .then((data) => {
    // Display bank info
    document.getElementById("bank-name").textContent = data.bank.bankName;
    document.getElementById("contact-info").textContent = data.bank.contactInfo;

    // Display offers
    const offersBody = document.getElementById("offers-body");
    offersBody.innerHTML = "";
    data.offers.forEach((offer) => {
      const row = document.createElement("tr");
      row.innerHTML = `
            <td>${offer.maximumAmount.toLocaleString()}</td>
            <td>${offer.interestRate}%</td>
            <td>${offer.minimumSalary.toLocaleString()}</td>
            <td><button onclick="deleteOffer(${
              offer.offerID
            })"><i class="fas fa-trash"></i> حذف</button></td>
          `;
      offersBody.appendChild(row);
    });

    // Display loan requests
    const requestsBody = document.getElementById("requests-body");
    requestsBody.innerHTML = "";
    data.loanRequests.forEach((req) => {
      const offer = data.offers.find((o) => o.offerID === req.offerID);
      const info = data.loanInfos.find((i) => i.offerID === req.offerID);

      requestsBody.innerHTML += `
            <tr>
              <td>مستخدم رقم ${req.userID}</td>
              <td>${info ? info.loanAmount.toLocaleString() : "-"}</td>
              <td>${req.submissionDate.split("T")[0]}</td>
              <td>${req.status}</td>
            </tr>
          `;
    });
  });

function deleteOffer(offerId) {
  fetch("/bank-dashboard/delete-offer", {
    method: "POST",
    headers: {
      "Content-Type": "application/x-www-form-urlencoded",
    },
    body: `offerId=${offerId}`,
  }).then(() => location.reload());
}

function addNewOffer() {
  const interest = prompt("نسبة الفائدة:");
  const maxAmount = prompt("المبلغ الأقصى:");
  const minSalary = prompt("الراتب الأدنى:");

  console.log("📤 Adding new offer:", { interest, maxAmount, minSalary });

  if (interest && maxAmount && minSalary) {
    fetch("/bank-dashboard/add-offer", {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
      body: `interestRate=${interest}&maxAmount=${maxAmount}&minSalary=${minSalary}`,
    })
      .then((res) => {
        console.log("✅ Response status:", res.status);
        if (res.ok) {
          location.reload();
        } else {
          alert("❌ فشل إضافة العرض. تأكد من إدخال بيانات صحيحة.");
        }
      })
      .catch((err) => {
        console.error("❌ Error adding offer:", err);
        alert("⚠️ حدث خطأ عند الإضافة.");
      });
  }
}

