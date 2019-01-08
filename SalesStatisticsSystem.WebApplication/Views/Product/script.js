function searchFilter() {
    var td, i;
    var input = document.getElementById('entryField');
    var filter = input.value.toUpperCase();
    var lookupTable = document.getElementById("lookupTable");
    var tr = lookupTable.getElementsByTagName('tr');

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementById("lol")[0];
        if (td.innerHTML.toUpperCase.indexOf(filter) > -1) {
            tr[i].style.display = "";
        } else {
            tr[i].style.display = "none";
        }
    }
}