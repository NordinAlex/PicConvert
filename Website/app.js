// Function to set a cookie with an expiration date
function setCookie(name, value, days) {
    const date = new Date();
    date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
    const expires = "expires=" + date.toUTCString();
    document.cookie = name + "=" + value + ";" + expires + ";path=/";
}

// Function to get a cookie by name
function getCookie(name) {
    const cname = name + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookieArray = decodedCookie.split(';');
    for (let i = 0; i < cookieArray.length; i++) {
        let cookie = cookieArray[i].trim();
        if (cookie.indexOf(cname) === 0) {
            return cookie.substring(cname.length, cookie.length);
        }
    }
    return "";
}

document.addEventListener("DOMContentLoaded", () => {
    const checkbox = document.getElementById('language-toggle');
    const cookieBanner = document.getElementById('cookieBanner');
    const cookieMessage = document.getElementById('cookieMessage');
    const acceptCookiesBtn = document.getElementById('acceptCookies');

    const logo = document.querySelector('.logo');
    const menuIcon = document.querySelector('.menu-icon');
    const navLinks = document.querySelector('.nav-links');
    const links = document.querySelectorAll('.nav-links a');  
    const PicConvertDemos = document.querySelectorAll('.demoImage');
    const modal = document.getElementById('image-modal');
    const modalImg = document.getElementById('modal-img');
    const closeBtn = document.querySelector('.close');

    // Function to toggle body scroll
    function toggleBodyScroll(disableScroll) {
        document.body.style.overflow = disableScroll ? 'hidden' : 'auto';
    }

    // Manage logo clicks - redirect to home
    logo.addEventListener('click', () => window.location.href = '/');

    // Manage menu icon clicks - toggle navigation menu
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

    // Close menu when clicking outside of menu
    document.addEventListener('click', (event) => {
        if (!navLinks.contains(event.target) && !menuIcon.contains(event.target)) {
            navLinks.classList.remove('active');
            toggleBodyScroll(false);
        }
    });

    // Open modal when clicking on a demo image
    PicConvertDemos.forEach(demoImage => {
        demoImage.addEventListener('click', () => {
            modal.style.display = 'flex';
            modalImg.src = demoImage.getAttribute('data-src');
            toggleBodyScroll(true);
        });
    });

    closeBtn.addEventListener('click', () => {
        modal.style.display = 'none';
        toggleBodyScroll(false);
    });

    // Close modal when clicking outside of modal
    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            modal.style.display = 'none';
            toggleBodyScroll(false);
        }
    });




    
    // Function to update cookie message based on language
    function updateCookieMessage(language) {
        if (language === 'en') {
            cookieMessage.textContent = "Cookies are used to ensure the best experience on the website.";
            acceptCookiesBtn.textContent = "Accept";
        } else {
            cookieMessage.textContent = "Cookies används för att säkerställa den bästa upplevelsen på webbplatsen.";
            acceptCookiesBtn.textContent = "Acceptera";
        }
    }

    // Function to handle language selection
    (function() {
        let language = getCookie('picConvert_language'); 

        // if no language cookie is set, check the URL
        if (!language) {
            language = window.location.pathname.includes('/en/') ? 'en' : 'sv';
            setCookie('picConvert_language', language, 3); // Save language in cookie
        }

        // Set the language toggle checkbox based on the language
        checkbox.checked = language === 'en';

        // Update cookie message based on language
        updateCookieMessage(language);

        // Om sidan och språket inte matchar, omdirigera
        if ((language === 'en' && !window.location.pathname.includes('/en/')) ||
            (language === 'sv' && window.location.pathname.includes('/en/'))) {
            window.location.href = language === 'en' ? '/PicConvert/en/index.html' : '/PicConvert/index.html';
        }

        // Update language when checkbox is toggled
        checkbox.addEventListener('change', () => {
            const newLang = checkbox.checked ? 'en' : 'sv';
            setCookie('picConvert_language', newLang, 3);
            window.location.href = newLang === 'en' ? '/PicConvert/en/index.html' : '/PicConvert/index.html';
        });
    })();

    // Event listener for cookie banner accept button
    acceptCookiesBtn.addEventListener("click", function() {
        cookieBanner.style.display = "none";
        // Save cookie to remember that the user has accepted cookies
        setCookie("picConvert_cookiesAccepted", "true", 3);
    });

    // Check if user has accepted cookies
    if (getCookie("picConvert_cookiesAccepted") === "true") {
        cookieBanner.style.display = "none";
    }
});
