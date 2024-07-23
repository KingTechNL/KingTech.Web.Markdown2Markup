//Dictionary to store the state of each slider.
var imageSliders = {};

//Next/previous controls
function plusSlides(identifier, n) {
    if (identifier in imageSliders) {
        showSlides(identifier, imageSliders[identifier] += n);
    } else {
        showSlides(identifier, imageSliders[identifier] = n);
    }
}

//Thumbnail image controls
function currentSlide(identifier, n) {
    console.log("Setting slider "+identifier+" to slide "+n);
    showSlides(identifier, imageSliders[identifier] = n);
}

//Set actual slide
function showSlides(identifier, n) {
    var i;
    var container = document.getElementById(identifier);
    var slides = container.getElementsByClassName("mySlides");
    var dots = container.getElementsByClassName("dot");

    //Set back to first slide
    if (n > slides.length) { imageSliders[identifier] = 1 }

    //Set to last slide
    if (n < 1) { imageSliders[identifier] = slides.length }

    //Hide all slides
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }

    //Unhighlight all thumbs
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }

    //Set active slide
    slides[imageSliders[identifier] - 1].style.display = "block";
    dots[imageSliders[identifier] - 1].className += " active";
}