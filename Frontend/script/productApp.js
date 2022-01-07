const baseRawUrl = "http://localhost:5000";
const baseUrl = `${baseRawUrl}/api`;

window.addEventListener("DOMContentLoaded", function(event){
    window.addEventListener('load', function(event){
        //load the page and basic elements
        const bookDescriptor = document.getElementById('book-presentator');
        const bookModalForm = document.getElementById('modal-book-edit');
        const bookPrice = document.getElementById('book-price-per-unit');
        const buyProductForm = document.getElementById('payment-form');
        var queryParams = window.location.search.split('?');
        var bookId= queryParams[1].split('=')[1];
        fetchGetBook(bookId)
        .then((book) => {
            console.log(book);
            bookDescriptor.innerHTML = getBookDescriptorHtml(book);
            bookPrice.textContent = book.price  + "$";
            buyProductForm.addEventListener('submit',function(event) {
                buyBook(book, event);
            });
            fetchGetEditorials()
            .then((editorialList) => {
                bookModalForm.innerHTML = getModalBookForm(book, editorialList);
            });
        });
    });
});

async function fetchGetBook(bookId){
    const getBookUrl = `${baseUrl}/books/${bookId}`;
    var response = await fetch(getBookUrl);
    var book = undefined
    if(response.status == 200){
        book = await response.json();
        return book;
    } else {
        var error = await response.text();
        alert(error);
    }
}

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

async function buyBook(book, event){
    event.preventDefault();
    const updateBookUrl =  `${baseUrl}/books/${book.id}/editorial/${book.editorialId}`;
    var bookPrice = book.price;

    var quantity = parseInt(event.currentTarget.quantity.value);
    var totalQuantity = book.quantitySold + quantity;

    var book = {
        quantitySold : totalQuantity
    };
s
    var bookJson = JSON.stringify(book);

    const params = {
        headers: { "Content-Type": "application/json; charset=utf-8" },
        method: 'POST',
        body: bookJson
    };

    var totalPrice = bookPrice * quantity;

    var response = await fetch(updateBookUrl, params);
    if(response.status == 200){
        alert(`Se te vendio ${quantity} copias a ${totalPrice}$`);
        location.reload();
    } else {
        var error = await response.text();
        alert(error);
    }
}

async function deleteBook(bookId){
    var deleteConfirmed = confirm("Seguro que quieres borrar este libro?");
    if(deleteConfirmed == true){
        const deleteBookUrl = `${baseUrl}/books/${bookId}`;
        const params ={
            method: 'DELETE'
        }

        var response = await fetch(deleteBookUrl, params);
        if(response.status == 200){
            alert("Libro borrado con exito!");
            window.location.href = `listProducts.html`
        } else {
            var error = await response.text();
            alert(error);
        }
    }
}

async function editBook(bookId){
    event.preventDefault();
    const editorialId = document.getElementById('book-editorial-input').value;
    const updateBookUrl =  `${baseUrl}/books/${bookId}/editorial/${editorialId}`;

    const name = document.getElementById('book-name-input');
    const author = document.getElementById('book-author-input')
    const description = document.getElementById('book-description-input');
    const genre = document.getElementById('book-genre-input');
    const price = document.getElementById('book-price-input');

    var book = {
        name : name.value,
        genre : genre.value,
        author : author.value,
        price : parseFloat(price.value),
        description : description.value
    }

    var bookJson = JSON.stringify(book);

    const params = {
        headers: { "Content-Type": "application/json; charset=utf-8" },
        method: 'POST',
        body: bookJson
    };

    var response = await fetch(updateBookUrl, params);
    if(response.status == 200){
        alert("Libro guardado con exito!");
        location.reload();
    } else {
        var error = await response.text();
        alert(error);
    }
}

async function editBookForm(bookId){
    event.preventDefault();
    const editorialId = document.getElementById('book-editorial-input').value;
    const updateBookUrl =  `${baseUrl}/books/${bookId}/editorial/${editorialId}/form`;

    const name = document.getElementById('book-name-input');
    const author = document.getElementById('book-author-input')
    const description = document.getElementById('book-description-input');
    const genre = document.getElementById('book-genre-input');
    const price = document.getElementById('book-price-input');
    const image = document.getElementById('book-imageFile-input');

    var floatPrice =  parseFloat(price.value).toString();

    const formBook = new FormData();
    formBook.append('Name', name.value);
    formBook.append('Genre', genre.value);
    formBook.append('Author', author.value);
    formBook.append('PriceForm', floatPrice);
    formBook.append('Description',description.value);
    formBook.append('Image', image.files[0]);

    const params ={
        method: 'POST',
        body: formBook
    }

    var response = await fetch(updateBookUrl, params);
    if(response.status == 200){
        alert("Libro guardado con exito!");
        location.reload();
    } else {
        var error = await response.text();
        alert(error);
    }
}


function getBookDescriptorHtml(book){
    const imageUrl = book.imagePath? `${baseRawUrl}/${book.imagePath}` : "";
    
    var bookDescriptor = `
    <h2 class="sub-title">${book.author} - ${book.name}</h2>
    <div class="book-descriptor">
        <img src="${imageUrl}">
        <h4 style="font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif; font-size: 2em;">Description</h2>
        <p>
           ${book.description}
        </p>
    </div>
    `;

    return bookDescriptor;
}

function getModalBookForm(book, editorialList){
    var bookForm = `
    <form id="formEditBook">
        <div class="form-group my-1 row">
            <label for="book-name-input" class="col-form-label py-1 col-sm-4">Name</label>
            <div class="col-sm-8">
                <input type="text" class="form-control form-control-sm" id="book-name-input" name="name" value="${book.name}">
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="book-author-input" class="col-form-label pb-0 col-sm-4">Author</label>
            <div class="col-sm-8">
                <input class="form-control form-control-sm" id="book-author-input" name="author" value="${book.author}">
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="book-description-input"
                class="col-form-label pb-0 col-sm-4">Description</label>
            <div class="col-sm-8">
                <textarea class="form-control form-control-sm" id="book-description-input" name="description">${book.description}</textarea>
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="book-genre-input"
                class="col-form-label pb-0 col-sm-4">Genre</label>
            <div class="col-sm-8">
                <input class="form-control form-control-sm" id="book-genre-input" name="genre" value="${book.genre}">
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="book-price-input" class="col-form-label py-1 col-sm-4">Price</label>
            <div class="col-sm-8">
                <input type="number" class="form-control form-control-sm" id="book-price-input" name="price" step="0.01" min="0" value="${book.price}">
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="book-editorial-input" class="col-form-label py-1 col-sm-4">Editorial</label>
            <div class="col-sm-8">
                <select class="form-control form-control-sm" id="book-editorial-input" name="editorial" value="${book.editorialId}">
                    ${getHtmlOptionsForEditorials(editorialList, book.editorialId)}
                </select>
            </div>
        </div>
        <div class="form-group my-1 row">
            <label for="book-imageFile-input" class="col-form-label py-1 col-sm-4">Image File</label>
            <div class="col-sm-8">
                <input type="file" class="form-control form-control-sm" id="book-imageFile-input"  name="image" accept="image/*">
            </div>
        </div>
        <div class="modal-footer my-0 py-1">
            <button type="button" class="btn btn-danger" data-bs-dismiss="modal" onclick="deleteBook(${book.id})">Delete</button>
            <button type="submit" id="btnSave" class="btn btn-success" onclick="editBookForm(${book.id})">Save</button>
        </div>
    </form>
    `;

    return bookForm;
}

function getHtmlOptionsForEditorials(editorialsList, editorialId){
    var editorialsOptionsHtml = "";
    editorialsList.forEach(editorial => {
        var editorialOption = ``;
        if(editorial.id == editorialId){
            editorialOption = `
                <option value="${editorial.id}" selected>${editorial.name}</option>
            `
        } else { 
            editorialOption = `
                <option value="${editorial.id}">${editorial.name}</option>
            `
        }

        editorialsOptionsHtml = editorialsOptionsHtml + editorialOption;
    });

    return editorialsOptionsHtml;
}