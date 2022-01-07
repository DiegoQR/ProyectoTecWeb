const baseRawUrl = "http://localhost:5000";
const baseUrl = `${baseRawUrl}/api`;

window.addEventListener('DOMContentLoaded', function(event){
    window.addEventListener('load', function(event) {
        //Load the page with the editorials
        const editorialsContainer = document.getElementById('editors-container');
        const createEditorialFrom = document.getElementById('form-add-editorial');

        fetchGetEditorials()
        .then((listEditorials) => {
            console.log(listEditorials);
            editorialsContainer.innerHTML =  getHtmlForMultipleEditorials(listEditorials);
        });
        //To create an editorial with the form modal
        createEditorialFrom.addEventListener('submit', fetchPostFormEditorial);
    });
});

async function fetchGetEditorials(){
    const getEditorialsUrl = `${baseUrl}/editorials`;

    let response = await fetch(getEditorialsUrl);
    var listEditorials = undefined;
    if(response.status == 200){
        listEditorials = await response.json();
        return listEditorials;
    } else {
        var error = await response.text();
        alert(error);
    }
}

async function fetchPostEditorial(event){
    const postEditorialUrl = `${baseUrl}/editorials`;

    var editorial = {
        name: event.currentTarget.name.value,
        description: event.currentTarget.description.value,
        address: event.currentTarget.address.value,
        country: event.currentTarget.country.value,
        eMail: event.currentTarget.eMail.value,
    }

    var editorialJson = JSON.stringify(editorial);

    const params ={
        headers: { "Content-Type": "application/json; charset=utf-8" },
        method: 'POST',
        body: editorialJson
    }

    let response = await fetch(postEditorialUrl, params);
    if(response.status == 201){
        alert("editorial creado!");
        location.reload();
    } else {
        var error = await response.text();
        alert(error);
    }
}

async function fetchPostFormEditorial(event){
    event.preventDefault();
    const postEditorialUrl = `${baseUrl}/editorials/form`;

    const formEditorial = new FormData();
    formEditorial.append('Name', event.currentTarget.name.value);
    formEditorial.append('Description', event.currentTarget.description.value);
    formEditorial.append('Address', event.currentTarget.address.value);
    formEditorial.append('Country', event.currentTarget.country.value);
    formEditorial.append('EMail', event.currentTarget.eMail.value);
    formEditorial.append('Image', event.currentTarget.image.files[0]);

    const params ={
        method: 'POST',
        body: formEditorial
    }

    let response = await fetch(postEditorialUrl, params);
    if(response.status == 201){
        alert("Editorial created!");
        location.reload();
    } else {
        var error = await response.text();
        alert(error);
    }
}


function getHtmlForMultipleEditorials(listEditorials){
    var editorialsListHtml = "";
    listEditorials.forEach(editorial => {
        editorialsListHtml = editorialsListHtml + getHtmlEditorial(editorial);
    });

    return editorialsListHtml;
}

function getHtmlEditorial(editorial){
    const imageUrl = editorial.imagePath? `${baseRawUrl}/${editorial.imagePath}` : "";

    var editorCellHtml = `
    <div class="editor-cell">
        <a href="editor.html?editorialId=${editorial.id}">
            <img src="${imageUrl}" alt="${editorial.name}">
        </a>
        <a href="editor.html?editorialId=${editorial.id}">${editorial.name}</a>
    </div>
    `

    return editorCellHtml;
}