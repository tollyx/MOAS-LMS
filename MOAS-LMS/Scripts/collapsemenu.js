function expandActivity(id) {
    let k = document.getElementById("activity" + id);
    let j = k.parentElement.parentElement.parentElement.parentElement;
    let l = document.getElementsByClassName("in");

    $(l).removeClass("in");
    $(j).collapse('show');
    $(k).collapse('show');
}