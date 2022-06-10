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
    dataTable = $('#tblWords').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                text: 'Go to Exercise',
                attr: {
                    id: 'button'
                }
            }
        ],
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
                "data": "status",
                "render": function (data) {


                    if (data == 1) {
                        return `
                        <a>*</a>
                        `
                    } else if (data == 2) {
                        return `
                        <a>**</a>
                        `
                    } else {
                        return `
                        <a>***</a>
                        `
                    }
                }, "autoWidth": "true"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="home/details/${data}" class="btn btn-primary m-1">${details}</a>
                    <a href="home/delete/${data}" class="btn btn-danger m-1"><i class="fa-solid fa-trash"></i></a>
                    `                }
            }
        ]
    });

    $('#tblWords tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });

    $('#button').click(function () {
        var selectedData = dataTable.rows('.selected').data();
        var selectedIds = []

        for (var i = 0; i < selectedData.length; i++) {
            selectedIds.push(selectedData[i].id);
        }

        console.log(selectedIds);

        $.ajax({
            type: "POST",
            url: "/Exercise/SaveExercise",
            data: { ids: selectedIds },
            dataType: "json",
            traditional: "true",
            success: function (data) {
                alert("SUCCESS");
            },
        })
    });
}
