// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById("toggle").onclick = () => {
    const careers = document.getElementById("careers");
    const toggle = document.getElementById("toggle");
    let status = careers.style.display === "none" ? "show" : "hide";
    if (status === "hide") {
        careers.style.display = "none";
        toggle.innerHTML = "Show Careers";
    } else {
        careers.style.display = "block";
        toggle.innerHTML = "Hide Careers";
    }
};