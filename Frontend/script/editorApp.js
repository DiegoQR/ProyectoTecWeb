const baseRawUrl = "http://localhost:5000";
const baseUrl = `${baseRawUrl}/api`;

window.addEventListener("DOMContentLoaded", function(event){
    window.addEventListener('load', function(event){
        //load the page and basic elements
        const editorialDescriptor = document.getElementById('editorial-body');
        const bookContainer = document.getElementById('books-container');
        const editorialForm = this.document.getElementById('edit-editorial-form');
        var queryParams = window.location.search.split('?');
        var editorialId= queryParams[1].split('=')[1];
        loadEditorialsPage(editorialDescriptor, bookContainer, editorialForm, editorialId);
    });
});

function loadEditorialsPage(editorialDescriptor, bookContainer, editorialForm, editorialId){
    fetchGetEditorialWithBooks(editorialId)
    .then((editorial) => {
        var editorialDescriptorHtml = getHtmlEditorialDescriptor(editorial);
        var bookOptionsHtml = getHtmlForMultipleBooks(editorial.books);
        var editorialFormHtml = getModalFormEditorial(editorial);
        editorialDescriptor.innerHTML = editorialDescriptorHtml;
        bookContainer.innerHTML = bookOptionsHtml;
        editorialForm.innerHTML = editorialFormHtml;
    });
}

async function fetchGetEditorialWithBooks(editorialId){
    const getEditorialUrl = `${baseUrl}/editorials/${editorialId}?showBooks=true`

    var response = await fetch(getEditorialUrl);
    var editorial = undefined
    if(response.status == 200){
        editorial = await response.json();
        return editorial;
    } else {
        var error = await response.text();
        alert(error);
    }
}

async function deleteEditorial(editorialId){
    var deleteConfirmed = confirm("Seguro que quieres borrar este editorial? Todos los libros que contengan esta editorial tambien seran borrados");
    if(deleteConfirmed == true){
        const deleteEditoiralUrl = `${baseUrl}/editorials/${editorialId}`;
        const params ={
            method: 'DELETE'
        }

        var response = await fetch(deleteEditoiralUrl, params);
        if(response.status == 200){
            alert("Editorial borrado con exito!");
            window.location.href = `listEditors.html`
        } else {
            var error = await response.text();
            alert(error);
        }
    }
}

async function editEditorial(editorialId){
    event.preventDefault();
    const updateEditorialUrl =  `${baseUrl}/editorials/${editorialId}`

    const name = document.getElementById('editorial-name-input');
    const description = document.getElementById('editorial-description-input');
    const address = document.getElementById('editorial-address-input');
    const country = document.getElementById('editorial-country-input');
    const eMail = document.getElementById('editorial-EMail-input');

    var editorial = {
        name : name.value,
        description : description.value,
        address : address.value,
        country : country.value,
        eMail : eMail.value
    }

    var editorialJson = JSON.stringify(editorial);

    const params = {
        headers: { "Content-Type": "application/json; charset=utf-8" },
        method: 'POST',
        body: editorialJson
    };

    var response = await fetch(updateEditorialUrl, params);
    if(response.status == 200){
        alert("Editorial guardado con exito!");
        location.reload();
    } else {
        var error = await response.text();
        alert(error);
    }
}

function getHtmlEditorialDescriptor(editorial){
    const imageUrl = editorial.imagePath? `${baseRawUrl}/${editorial.imagePath}` : "";

    var editorCellHtml = `
    <div class="editorial-presentation">
        <img src="${imageUrl}" alt="${editorial.name}">
        <h2 class="sub-title">${editorial.name}</h2>
    </div>
    <div class="editorial-descriptor">
        <h2>Who are we?</h2>
        <p>
            ${editorial.description}
        </p>
        <h2>Contact us:</h2>
        <ul>
            <li>Country: ${editorial.country} </li>
            <li>Address: ${editorial.address} </li>
            <li>E-Mail: ${editorial.eMail}</li>
        </ul>
    </div>
    `

    return editorCellHtml;
}

function getHtmlForMultipleBooks(listBooks){
    var booksListHtml = "";
    listBooks.forEach(book => {
        booksListHtml = booksListHtml + getHtmlBookOption(book);
    });

    return booksListHtml;
}

function getHtmlBookOption(book){
    const imageUrl = book.imagePath? `${baseRawUrl}/${book.imagePath}` : "";

    var bookOptionHtml = `
    <div class="book-option">
        <img src="${imageUrl}">
        <p class="book-title">${book.name}</p>
        <p class="book-author">${book.author}</p>
        <p class="book-price">${book.price} <i class="currency-symbol">$</i> </p>
        <button onclick="document.location='product.html'">View Product</button>
    </div>
    `

    return bookOptionHtml;
}

function getModalFormEditorial(editorial){
    var editorialFormHtml = `
    <form id="formEditEditorial">
        <div class="form-group my-1 row">
            <label for="editorial-name-input" class="col-form-label py-1 col-sm-4">Name</label>
            <div class="col-sm-8">
                <input type="text" class="form-control form-control-sm" id="editorial-name-input" name="name" value="${editorial.name}">
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="editorial-description-input" class="col-form-label pb-0 col-sm-4">Description</label>
            <div class="col-sm-8">
                <textarea class="form-control form-control-sm" id="editorial-description-input" name="description">${editorial.description}</textarea>
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="editorial-address-input" class="col-form-label pb-0 col-sm-4">Address</label>
            <div class="col-sm-8">
                <input class="form-control form-control-sm" id="editorial-address-input" name="address" value="${editorial.address}">
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="editorial-country-input" class="col-form-label py-1 col-sm-4">Country</label>
            <div class="col-sm-8">
                <input class="form-control form-control-sm" id="editorial-country-input" name="country" value="${editorial.country}">
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="editorial-EMail-input" class="col-form-label py-1 col-sm-4">E-mail</label>
            <div class="col-sm-8">
                <input  class="form-control form-control-sm" id="editorial-EMail-input" name="eMail" value="${editorial.eMail}">
            </div>
        </div>
        <div class="modal-footer my-0 py-1">
            <button type="button" class="btn btn-danger" data-bs-dismiss="modal" onclick="deleteEditorial(${editorial.id})">Delete</button>
            <button type="submit" id="btnSave" class="btn btn-success" onclick="editEditorial(${editorial.id})">Save</button>
        </div>
    </form>
    `
    return editorialFormHtml;
}