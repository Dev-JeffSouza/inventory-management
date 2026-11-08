
window.addEventListener('load', function () {
    var loader = document.getElementById('loading-screen');

    if (loader) {

        loader.style.transition = 'opacity 0.3s ease';
        loader.style.opacity = '0';
        setTimeout(function() {
            loader.style.display = 'none';
        }, 300);
    }
});

