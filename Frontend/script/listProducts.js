const baseRawUrl = "http://localhost:5000";
const baseUrl = `${baseRawUrl}/api`;

window.addEventListener('DOMContentLoaded', function(event){
    window.addEventListener('load', function(event) {
        const categoryNavigator = document.getElementById('category-navigator');
        const booksGenreContainer = document.getElementById('book-genre-container');
        const editorialsCombobox = document.getElementById('book-editorial-input');
        const createBookForm = document.getElementById('form-add-book');
        //Get list of books and generes
        fetchGetAllBooks()
        .then((allBooksList) => {
            console.log(allBooksList);
            var generesList = getGeneres(allBooksList);
            categoryNavigator.innerHTML = getCategoryNavigatorHtml(generesList);
            booksGenreContainer.innerHTML = getHtmlForMultipleGenres(generesList, allBooksList);
        });
        //Get editorial options to post a book
        fetchGetEditorials()
        .then((editroialsList) => {
            editorialsCombobox.innerHTML = getHtmlOptionsForEditorials(editroialsList);
        });
        //For creating a new book
        createBookForm.addEventListener('submit',fetchPostFormBook);   
    });
});

async function fetchGetAllBooks(){
    const getAllBooksUrl = `${baseUrl}/books`;

    const params = {
        headers: { "Authorization": `Bearer ${sessionStorage.getItem("jwt")}` },
        method: "GET"
    }

    let response = await fetch(getAllBooksUrl, params);
    var listBooks = undefined;
    if(response.status == 200){
        listBooks = await response.json();
        return listBooks;
    } else {
        var error = await response.text();
        alert(error)
    }
}

async function fetchGetEditorials(){
    const getEditorialsUrl = `${baseUrl}/editorials`;

    const params = {
        headers: { "Authorization": `Bearer ${sessionStorage.getItem("jwt")}` },
        method: "GET"
    }

    let response = await fetch(getEditorialsUrl, params);
    var listEditorials = undefined;
    if(response.status == 200){
        listEditorials = await response.json();
        return listEditorials;
    } else {
        var error = await response.text();
        alert(error);
    }
}

async function fetchPostFormBook(event){
    event.preventDefault();
    const postBookUrl = `${baseUrl}/books/editorial/form/${event.currentTarget.editorial.value}`;

    const formBook = new FormData();
    formBook.append('Name', event.currentTarget.name.value);
    formBook.append('Genre', event.currentTarget.genre.value);
    formBook.append('Author', event.currentTarget.author.value);
    formBook.append('PriceForm', parseFloat(event.currentTarget.price.value).toString());
    formBook.append('Description', event.currentTarget.description.value);
    formBook.append('QuantitySold', 0);
    formBook.append('Image', event.currentTarget.image.files[0]);

    const params ={
        headers: { "Authorization": `Bearer ${sessionStorage.getItem("jwt")}` },
        method: 'POST',
        body: formBook
    }

    let response = await fetch(postBookUrl, params);
    if(response.status == 201){
        alert("Book created!");
        location.reload();
    } else {
        var error = await response.text();
        alert(error);
    }
}

function getGeneres(bookList){
    generes = new Set();
    bookList.forEach(book => {
        generes.add(book.genre);
    });

    return Array.from(generes);
}

function getCategoryNavigatorHtml(categoryList){
    var allCategoriesHtml = "";
    categoryList.forEach(category => {
        var categoryHtml = `
            <a href="#${category.replace(' ','-').toLowerCase()}">${category}</a>
        `
        allCategoriesHtml = allCategoriesHtml + categoryHtml;
    });
    return allCategoriesHtml;
}


function getHtmlForMultipleGenres(listGeneres, bookList) {
    var generesListWithBooksHtml = "";
    listGeneres.forEach(genre => {
        var genereWithBooksHtml = `
        <h2 class="sub-title" id="${genre.replace(' ','-').toLowerCase()}">${genre}</h2>
        <div class="books-container">
            ${getHtmlForMultipleBooks(bookList.filter(book => book.genre == genre))}
        </div>
        `
        generesListWithBooksHtml = generesListWithBooksHtml + genereWithBooksHtml;
    });
    return generesListWithBooksHtml;
}


function getHtmlForMultipleBooks(listBooks){
    var booksListHtml = "";
    listBooks.forEach(book => {
        const imageUrl = book.imagePath? `${baseRawUrl}/${book.imagePath}` : "";

        var bookOptionHtml = `
        <div class="book-option">
            <img src="${imageUrl}">
            <p class="book-title">${book.name}</p>
            <p class="book-author">${book.author}</p>
            <p class="book-price">${book.price} <i class="currency-symbol">$</i> </p>
            <button onclick="document.location='product.html?bookId=${book.id}'">View Product</button>
        </div>
        `

        booksListHtml = booksListHtml + bookOptionHtml
    });

    return booksListHtml;
}

function getHtmlOptionsForEditorials(editorialsList){
    var editorialsOptionsHtml = "";
    editorialsList.forEach(editorial => {
       
        var editorialOption = `
            <option value="${editorial.id}">${editorial.name}</option>
        `

        editorialsOptionsHtml = editorialsOptionsHtml + editorialOption;
    });

    return editorialsOptionsHtml;
}