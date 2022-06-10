function tblWordsTo_tblEx() {
    var tableWords = document.getElementById("tblWords"),
        tableExercise = document.getElementById("tblEx"),
        checkboxes = document.getElementsByName("check-tblWords");

    for (var i = 0; i < checkboxes; i++) {
        if (checkboxes[i].checked) {
            var newRow = tableExercise.insertRow(tableExercise.length),
                cell1 = newRow.insertCell(0),
                cell2 = newRow.insertCell(1);

            cell1.innerHTML = tableWords.rows[i].cells[0].innerHTML;
            cell2.innerHTML = `<input type="checkbox" name="check-tblEx">`;
            console.log("yes");
        }
    }
}

/*<input type="checkbox" name="">*/
