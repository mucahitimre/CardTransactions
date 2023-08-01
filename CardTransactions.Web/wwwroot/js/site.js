// Liste öğeleri için boş bir dizi tanımlıyoruz
let itemList = [];

// Liste sayfasındaki HTML elemanları seçme
const itemListElement = document.getElementById('item-list');
const idFilterInput = document.getElementById('id-filter');
const dateFilterInput = document.getElementById('date-filter');
const filterButton = document.getElementById('filter-button');

// Detay sayfasındaki HTML elemanları seçme
const detailContainer = document.getElementById('detail-container');

// Verileri API'den al ve listeyi oluşturarak sayfada göster
function fetchData() {
    fetch('/Home/GetData') // API URL'ini buraya yazın
        //.then(response => response.json())
        .then(data => {
            itemList = data;
            renderList();
        })
        .catch(error => {
            console.error('Veri alınamadı:', error);
        });
}

// Listeyi oluştur ve sayfada göster
function renderList() {
    itemListElement.innerHTML = '';

    itemList.forEach(item => {
        const listItem = document.createElement('li');
        listItem.textContent = `ID: ${item.id} - Tarih: ${item.date}`;
        listItem.setAttribute('data-detail', item.detail);

        listItem.addEventListener('click', () => {
            showDetail(item.id);
        });

        itemListElement.appendChild(listItem);
    });
}

// Filtreleme işlemini gerçekleştir
function filterList() {
    const filteredItems = itemList.filter(item => {
        const idFilter = idFilterInput.value.trim();
        const dateFilter = dateFilterInput.value;

        const idMatch = idFilter === '' || item.id.toString() === idFilter;
        const dateMatch = dateFilter === '' || item.date === dateFilter;

        return idMatch && dateMatch;
    });

    itemListElement.innerHTML = '';

    filteredItems.forEach(item => {
        const listItem = document.createElement('li');
        listItem.textContent = `ID: ${item.id} - Tarih: ${item.date}`;
        listItem.setAttribute('data-detail', item.detail);

        listItem.addEventListener('click', () => {
            showDetail(item.id);
        });

        itemListElement.appendChild(listItem);
    });
}

// Detayları göster
function showDetail(itemId) {
    const selectedItem = itemList.find(item => item.id === itemId);

    if (selectedItem) {
        detailContainer.innerHTML = `<p>ID: ${selectedItem.id}</p><p>Tarih: ${selectedItem.date}</p><p>Detay: ${selectedItem.detail}</p>`;
    } else {
        detailContainer.innerHTML = '<p>Detay bulunamadı.</p>';
    }
}

// Sayfa yüklendiğinde veriyi al ve listeyi göster
document.addEventListener('DOMContentLoaded', () => {
    fetchData();
});

// Filtreleme butonuna tıklandığında listeyi filtrele
filterButton.addEventListener('click', () => {
    filterList();
});
