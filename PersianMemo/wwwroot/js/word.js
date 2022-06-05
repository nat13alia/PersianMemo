var dataTable;

function playAudio(url) {
    new Audio(url).play();
}

$(document).ready(function () {
    var language = document.querySelector('#lang').value;
    var details;
    if (language === "PL") {
        details = "Szczegóły"
    } else {
        details = "Details";
    }
    loadDataTable(details);
});


function loadDataTable(details) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Home/GetAll"
        },
        "columns": [
            { "data": "persianWord", "autoWidth": "true" },
            { "data": "translation", "autoWidth": "true" },
            { "data": "difficulty", "autoWidth": "true" },
            {
                "data": "pronunciationPath",
                "render": function (data) {
                    

                    if (data != null) {
                        return `
                        <a onclick="playAudio('/audio/${data}')"><i class="fa-solid fa-play"></i></a>
                        `
                    } else {
                        return `
                        <a><i class="fa-solid fa-volume-slash"></i></a>
                        `
                    }
                }, "autoWidth": "true"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="home/details/${data}" class="btn btn-primary m-1">${details}</a>
                    <a href="home/edit/${data}" class="btn btn-primary m-1"><i class="fa-solid fa-pen-to-square"></i></a>
                    <a href="home/delete/${data}" class="btn btn-danger m-1"><i class="fa-solid fa-trash"></i></a>
                    <a href="#" class="btn btn-danger m-1"><i class="fa-solid fa-graduation-cap"></i></a>
                    `
                }
            }
        ]
    });
}