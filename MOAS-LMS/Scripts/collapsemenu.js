var coll = document.getElementsByClassName("collapsible");

for (var i = 0; i < coll.length; i++) {
    coll[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var content = this.nextElementSibling;
        if (content.style.display === "block") {
            content.style.display = "none";
        } else {
            content.style.display = "block";
        }
    });
}


function expandActivity(id) {
    let k = document.getElementById("activity" + id);
    k.parentElement.parentElement.style.display = "block";

    for (var i = 0; i < k.childNodes.length; i++) {
        if (k.childNodes[i].className === "content") {
            k.childNodes[i].style.display = "block";
        }
    }   
}