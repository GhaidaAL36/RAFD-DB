async function loadUserInfo() {
  try {
    const response = await fetch('http://localhost:5102/auth/get-user-info');
    if (!response.ok) {
      throw new Error("Failed to fetch user info");
    }

    const user = await response.json();

    // Set user info in the table
    document.querySelector(
      ".user-title h1"
    ).innerText = `مرحبًا بك، ${user.userName}`;
    document.querySelectorAll(".user-info-table td")[0].innerText =
      user.userSalary ? `${user.userSalary} ريال` : "غير متوفر";
    document.querySelectorAll(".user-info-table td")[1].innerText =
      user.userJopType || "غير متوفر";
    document.querySelectorAll(".user-info-table td")[2].innerText =
      user.userContactNum || "غير متوفر";
    document.querySelectorAll(".user-info-table td")[3].innerText =
      user.userEmail || "غير متوفر";
    document.querySelectorAll(".user-info-table td")[4].innerText = user.userAge
      ? `${user.userAge} سنة`
      : "غير متوفر";
  } catch (error) {
    console.error("Error loading user info:", error);
  }
}


//hide and show nav bar items 
async function updateNavbar() {
    try {
        const response = await fetch('/auth/is-logged-in');
        if (!response.ok) {
            throw new Error('Failed to check login status');
        }

        const data = await response.json();

        // ✅ Use correct class names
        const loginBtn = document.querySelector('.login-btn');
        const createAccountBtn = document.querySelector('.create-account-btn');
        const dashboardBtn = document.querySelector('.dashboard-btn');
        const logoutBtn = document.querySelector('.logout-btn');

        if (data.isLoggedIn) {
            loginBtn.style.display = 'none';
            createAccountBtn.style.display = 'none';
            dashboardBtn.style.display = 'inline-block';
            logoutBtn.style.display = 'inline-block';
        } else {
            loginBtn.style.display = 'inline-block';
            createAccountBtn.style.display = 'inline-block';
            dashboardBtn.style.display = 'none';
            logoutBtn.style.display = 'none';
        }
    } catch (error) {
        console.error('Error updating navbar:', error);
    }
}

window.onload = () => {
    loadUserInfo();
    updateNavbar();
};