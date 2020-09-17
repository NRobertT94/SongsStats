
$(function () {

    $("#search").click(function () {

        $("#results-panel").show();
        $("#loading-text").show();
        $("#stats").hide();
        $("#error").hide();

        var artistName = $("#artist-text-box").val();
        var serviceUrl = "Artist/GetArtistSongsStats/";
      
        $.ajax({
            type: "GET",
            url: serviceUrl,
            data: { artist: artistName },  
            startTime: performance.now(),
            success: successFunc,
            error: errorFunc
        });

        function successFunc(data) {

            var time = performance.now() - this.startTime;
            var seconds = time / 1000;           
            seconds = seconds.toFixed(2);

            $("#loading-text").hide();

            $("#name").text(artistName);
            $("#songs-count").text(data.songsCount);
            $("#avg-words").text(data.averageWordsInSongs);
            $("#shortest-song-title").text(data.shortestSong);
            $("#shortest-song-words").text(data.shortestSongWordCount);
            $("#longest-song-title").text(data.longestSong);
            $("#longest-song-words").text(data.longestSongWordCount);

            $("#response-time").text(seconds);

            $("#stats").show();
        }

        function errorFunc() {
            $("#loading-text").hide();
            $("#error").show();
        }
    });
});