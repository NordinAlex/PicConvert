document.addEventListener("DOMContentLoaded", function() {
    const logo = document.querySelector('.logo');
    const menuIcon = document.querySelector('.menu-icon');
    const navLinks = document.querySelector('.nav-links');
    const links = document.querySelectorAll('.nav-links a');

    logo.addEventListener('click', function() {
        window.location.href = '/';
    });

    menuIcon.addEventListener('click', () => {
        navLinks.classList.toggle('active');
        toggleBodyScroll(navLinks.classList.contains('active'));
    });

    // Close the menu when a link is clicked
    links.forEach(link => {
        link.addEventListener('click', () => {
            navLinks.classList.remove('active');
            toggleBodyScroll(false);
        });
    });

    // close the menu when clicking outside
    document.addEventListener('click', (event) => {
        if (!navLinks.contains(event.target) && !menuIcon.contains(event.target)) {
            navLinks.classList.remove('active');
            toggleBodyScroll(false);
        }
    });

    // Toggle body scroll
    function toggleBodyScroll(disableScroll) {
        if (disableScroll) {
            document.body.style.overflow = 'hidden';
        } else {
            document.body.style.overflow = 'auto';
        }
    }
});







document.addEventListener('DOMContentLoaded', function() {
    const checkbox = document.getElementById('language-toggle');

    // Check if the user has selected a language
    const language = localStorage.getItem('language');
    
    if (language === 'en') {
        checkbox.checked = true;
        // Redirect to the English version of the page if needed
        if (window.location.pathname !== '/PicConvert/en/index.html') {
            window.location.href = '/PicConvert/en/index.html';
        }
    } else {
        checkbox.checked = false;
        // Redirect to the Swedish version of the page if needed
        if (window.location.pathname !== '/PicConvert/index.html') {
            window.location.href = '/PicConvert/index.html';
        }
    }

    checkbox.addEventListener('change', function() {
        if (this.checked) {
            // Set language to English
            localStorage.setItem('language', 'en');
            window.location.href = '/PicConvert/en/index.html'; // Redirect to the English page
        } else {
            // Set language to Swedish
            localStorage.setItem('language', 'sv');
            window.location.href = '/PicConvert/index.html'; // Redirect to the Swedish page
        }
    });
});
