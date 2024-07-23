document.addEventListener("DOMContentLoaded", function (event) {
    //Load YouTube Videos on page...
    var youTubeVideos = document.querySelectorAll('.youtube');
    for (var i = 0; i < youTubeVideos.length; i++) {
        var thumbnail = "https://img.youtube.com/vi/" + youTubeVideos[i].dataset.embed + "/maxresdefault.jpg";
        //set CSS
        youTubeVideos[i].style.cssText = "max-width: 560px;margin: 60px auto;";

        //set microdata attributes for SEO
        youTubeVideos[i].setAttribute("itemprop", "video");
        youTubeVideos[i].setAttribute("itemscope", '');
        youTubeVideos[i].setAttribute("itemtype", "http://schema.org/VideoObject");

        //set HTML
        youTubeVideos[i].innerHTML = '<div class="play"></div>' +
            '<meta itemprop="embedURL" content="https://www.youtube.com/embed/' + youTubeVideos[i].dataset.embed + '" />' +
            '<img style="cursor: pointer;" width="560" height="315" src="' + thumbnail + '" />';

        //add click event that will load YouTube video
        youTubeVideos[i].addEventListener("click", function () {
            console.log("Loading video " + this.dataset.embed);
            this.innerHTML = '<iframe width="560" height="315" frameBorder="0" ' +
                'allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture"' +
                'src="https://www.youtube.com/embed/' + this.dataset.embed + '?enablejsapi=1&rel=0&showinfo=0&autoplay=1"' +
                ' allowFullScreen="allowfullscreen"></iframe>';
        });
    }
});

function loadYoutube(element, videoId) {
    element.innerHTML = '<iframe width="560" height="315" frameBorder="0" ' +
        'allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture"' +
        'src="https://www.youtube.com/embed/' + videoId + '?enablejsapi=1&rel=0&showinfo=0&autoplay=1"' +
        ' allowFullScreen="allowfullscreen"></iframe>';
}