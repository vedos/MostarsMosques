
function showHide(value) {
    var input = document.getElementById('td+' + value);
    if (input.getAttribute("type") === "password") {
        show(value);
    } else {
        hide(value);
    }
}
function show(value) {
    var p = document.getElementById('td+' + value);
    p.setAttribute('type', 'text');
}

function hide(value) {
    var p = document.getElementById('td+' + value);
    p.setAttribute('type', 'password');
}

function Submit(id) {
    if (confirm("Da li ste sigurni da želite ovo izbrisati?")) {
        return true;
    } else {
        return false;
    }
}

function queryParams() {
    return {
        type: 'owner',
        sort: 'updated',
        direction: 'desc',
        per_page: 100,
        page: 1
    };
}

//Modal popup
