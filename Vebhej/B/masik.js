let tanulok;

let BetoltCim = () => {
    let tr = document.createElement('tr'), th;

    ['OM azonosító', 'Név', 'Matek', 'Magyar', 'Összesen'].forEach((cim) => {
        th = document.createElement('th');
        th.innerText = cim;
        tr.appendChild(th);
    });

    document.querySelector('#table').appendChild(tr);
};

let BetoltAdatok = () => {
    let tr, td,
    table = document.querySelector('#table'),
    minimum = document.querySelector('#pontszamMinimum').value || 0;

    document.querySelectorAll('#table tr:has(td)').forEach(tr => table.removeChild(tr))

    tanulok.filter(G => G.Összesen >= minimum).forEach((element) => {
        tr = document.createElement('tr');

        for (const key in element) {
            td = document.createElement('td');
            td.innerText = element[key];
            tr.appendChild(td);
        }

        table.appendChild(tr);
    });
};

let ReadFile = input => {
    let file = input.files[0],
        reader = new FileReader();

    reader.readAsText(file);

    reader.onload = () => {
        tanulok = [];

        lista = JSON.parse(reader.result);
        lista.forEach(element => {
            let ujElem = {
                'OM azonosító': element['OM_Azonosito'],
                'Név': element.Neve,
                'Matek': element.Matematika,
                'Magyar': element.Magyar,
                'Összesen': `${element.Matematika * 1 + element.Magyar}`
            };

            tanulok.push(ujElem);
        });

        console.log(tanulok);

        tanulok = tanulok.sort((a,b) => a.Név>b.Név ? 1 : a.Név<b.Név ? -1 : 0);
        BetoltAdatok();
    };

    reader.onerror = () => console.log(reader.error);
};