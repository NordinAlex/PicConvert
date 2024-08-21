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

    // Check if the user has a language preference stored in localStorage
    let language = localStorage.getItem('language');

    // If localStorage is empty, detect the current page language based on URL
    if (!language) {
        if (window.location.pathname.includes('/en/')) {
            language = 'en';
            localStorage.setItem('language', 'en');
        } else {
            language = 'sv';
            localStorage.setItem('language', 'sv');
        }
    }

    // Update the checkbox based on the language
    checkbox.checked = language === 'en';

    // If the user is on the wrong language page, redirect them
    if (language === 'en' && !window.location.pathname.includes('/en/')) {
        window.location.href = '/en/index.html';
    } else if (language === 'sv' && window.location.pathname.includes('/en/')) {
        window.location.href = '/index.html';
    }

    // Listen for changes to the checkbox and redirect accordingly
    checkbox.addEventListener('change', function() {
        if (this.checked) {
            // Set language to English
            localStorage.setItem('language', 'en');
            window.location.href = '/en/index.html'; // Redirect to the English page
        } else {
            // Set language to Swedish
            localStorage.setItem('language', 'sv');
            window.location.href = '/index.html'; // Redirect to the Swedish page
        }
    });
});
