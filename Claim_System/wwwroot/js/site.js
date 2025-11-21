// Auto-dismiss Bootstrap alerts after 5 seconds
document.addEventListener("DOMContentLoaded", function () {
    const alertList = document.querySelectorAll('.alert');
    alertList.forEach(function (alert) {
        setTimeout(() => {
            try {
                const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                bsAlert.close();
            } catch (e) { /* ignore */ }
        }, 5000);
    });
});

// Confirm before Approve/Reject buttons
function confirmAction(message, formId) {
    if (confirm(message)) {
        var form = document.getElementById(formId);
        if (form) form.submit();
    }
}

// Highlight active nav menu
document.addEventListener("DOMContentLoaded", function () {
    const currentUrl = window.location.pathname.toLowerCase().replace(/\/$/, "");
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    navLinks.forEach(link => {
        try {
            const href = link.getAttribute('href').toLowerCase().replace(/\/$/, "");
            if (href === currentUrl) {
                link.classList.add('active');
            }
        } catch (e) { /* ignore */ }
    });
});
