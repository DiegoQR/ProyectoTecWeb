const baseRawUrl = "http://localhost:5000";
const baseUrl = `${baseRawUrl}/api`;

window.addEventListener("DOMContentLoaded", function(event){
    window.addEventListener('load', function(event){
        const featuredBooks = document.getElementById('featured-books');
        fetchGetAllBooks()
        .then((allBooksList) => {
            var featuredBooksList = allBooksList.filter(book => book.quantitySold >= 10);
            featuredBooks.innerHTML = getHtmlForMultipleBooks(featuredBooksList);
        })
    });
});

async function fetchGetAllBooks(){
    const getAllBooksUrl = `${baseUrl}/books`;

    let response = await fetch(getAllBooksUrl);
    var listBooks = undefined;
    if(response.status == 200){
        listBooks = await response.json();
        return listBooks;
    } else {
        var error = await response.text();
        alert(error);
    }
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