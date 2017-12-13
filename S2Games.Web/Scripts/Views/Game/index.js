function lendGame() {
    var friendId = $('#Friends').val();
    var gameId = $('#GameId').val();

    $.ajax({
        url: '/Game/LendGame',
        data: { friendId: friendId, gameId: gameId },
        async: true,
        cache: false,
        success: function (data) {
            $('#AlertModal').modal();
            $('#AlertContent').text(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#AlertModal').modal();
            $('#AlertContent').text(data);
            console.error("lendGame: " + errorThrown);
        },
    });
}

function returnGame(gameId) {  

    $.ajax({
        url: '/Game/ReturnGame',
        data: { gameId: gameId },
        async: true,
        cache: false,
        success: function (data) {
            $('#AlertModal').modal();
            $('#AlertContent').text(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#AlertModal').modal();
            $('#AlertContent').text(data);
            console.error("lendGame: " + errorThrown);
        },
    });
}

function setGameId(gameId) {
    $('#GameId').val(gameId);
}