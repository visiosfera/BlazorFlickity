let dotnetReference;
let flkty;
let itemSelected;

export function init(reference) {

    dotnetReference = reference;

    flkty = new Flickity('.main-carousel', {
        // options
        draggable: false,
        cellAlign: 'center',
        freeScroll: true,
        groupCells: false,
        wrapAround: true,
        contain: true,
        arrowShape: {
            x0: 20,
            x1: 60, y1: 40,
            x2: 60, y2: 25,
            x3: 35
        }
    });

    //flkty.on('select', function (index) {
    //    console.log('select = ' + index);
    //});

    flkty.on('settle', function (index) {
        itemSelected = index;
    });

    flkty.on('staticClick', function (event, pointer, cellElement, cellIndex) {
        if (!cellElement) {
            return;
        }

        if (cellIndex == itemSelected) {
            dotnetReference.invokeMethodAsync("OnItemSelectedCallback", itemSelected);
        }
    });
}

export function goToSlide(position) {
    flkty.selectCell(position);
}