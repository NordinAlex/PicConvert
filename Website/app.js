document.addEventListener("DOMContentLoaded", () => {
    const logo = document.querySelector('.logo');
    const menuIcon = document.querySelector('.menu-icon');
    const navLinks = document.querySelector('.nav-links');
    const links = document.querySelectorAll('.nav-links a');
    const checkbox = document.getElementById('language-toggle');
    const screenshots = document.querySelectorAll('.screenshot');
    const modal = document.getElementById('image-modal');
    const modalImg = document.getElementById('modal-img');
    const closeBtn = document.querySelector('.close');

    // Function to toggle body scroll
    function toggleBodyScroll(disableScroll) {
        document.body.style.overflow = disableScroll ? 'hidden' : 'auto';
    }

    // Handle logo click - Redirect to home
    logo.addEventListener('click', () => window.location.href = '/');

    // Handle menu icon click - Toggle navigation menu
    menuIcon.addEventListener('click', () => {
        navLinks.classList.toggle('active');
        toggleBodyScroll(navLinks.classList.contains('active'));
    });

    // Close menu when a link is clicked
    links.forEach(link => {
        link.addEventListener('click', () => {
            navLinks.classList.remove('active');
            toggleBodyScroll(false);
        });
    });

    // Close menu when clicking outside of it
    document.addEventListener('click', (event) => {
        if (!navLinks.contains(event.target) && !menuIcon.contains(event.target)) {
            navLinks.classList.remove('active');
            toggleBodyScroll(false);
        }
    });

    // Language toggle functionality
    (function() {
        let language = localStorage.getItem('language');

        if (!language) {
            language = window.location.pathname.includes('/en/') ? 'en' : 'sv';
            localStorage.setItem('language', language);
        }

        checkbox.checked = language === 'en';

        if ((language === 'en' && !window.location.pathname.includes('/en/')) ||
            (language === 'sv' && window.location.pathname.includes('/en/'))) {
            window.location.href = language === 'en' ? '/PicConvert/en/index.html' : '/PicConvert/index.html';
        }

        checkbox.addEventListener('change', () => {
            const newLang = checkbox.checked ? 'en' : 'sv';
            localStorage.setItem('language', newLang);
            window.location.href = newLang === 'en' ? '/PicConvert/en/index.html' : '/PicConvert/index.html';
        });
    })();

    // Modal functionality for screenshots
    screenshots.forEach(screenshot => {
        screenshot.addEventListener('click', () => {
            modal.style.display = 'flex';
            modalImg.src = screenshot.getAttribute('data-src');
            toggleBodyScroll(true);
        });
    });

    closeBtn.addEventListener('click', () => {
        modal.style.display = 'none';
        toggleBodyScroll(false);
    });

    // Close modal when clicking outside of it
    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            modal.style.display = 'none';
            toggleBodyScroll(false);
        }
    });
});
