let tanulok;
let letolteshez;
let lastSort;

let BetoltCim = () => {
    let tr = document.createElement('tr'), th;

    ['OM azonosító', 'Név', 'Matek', 'Magyar', 'Összesen'].forEach((cim) => {
        th = document.createElement('th');
        th.innerText = cim;
        th.classList.add('hover');

        if (cim === 'Név')
            th.id = cim;

        th.addEventListener('click', (e) => {
            let property = e.target.innerText.replace(/(🡹|🡻)/, '');
            tanulok.sortBy(property, property !== 'Név' ? 'Név' : 'OM azonosító');
            ChangeLastSort(property);
            BetoltAdatok();
        })

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

    CsvGeneral();
};

Array.prototype.sortBy = function (property1, property2 = 'Név') {
    if (lastSort === property1) {
        return this.reverse();
    }

    return this.sort((a, b) => {
        let propA = a[property1];
        let propB = b[property1];

        if (typeof propA === 'string' && typeof propB === 'string') {
            propA = propA.toLowerCase();
            propB = propB.toLowerCase();
        }

        let res = propA < propB ? -1 : propA > propB ? 1 : 0;

        if (res === 0 && property1 !== property2) {
            let prop2A = a[property2];
            let prop2B = b[property2];

            if (typeof prop2A === 'string' && typeof prop2B === 'string') {
                prop2A = prop2A.toLowerCase();
                prop2B = prop2B.toLowerCase();
            }

            res = prop2A < prop2B ? -1 : prop2A > prop2B ? 1 : 0;
        }

        return res;
    });
};

let ChangeLastSort = property => {
    let old = [...document.querySelectorAll('#table th')].find(G => G.innerText === lastSort) || document.getElementById('Név');

    if (property !== lastSort) {
        old.innerText = lastSort ?? 'Név';
        [...document.querySelectorAll('#table th')].find(G => G.innerText === property).innerText = property;
    } else {
        old.innerText = property;
    }

    lastSort = property;
}

let ReadFile = input => {
    let file = input.files[0],
        reader = new FileReader();

    reader.readAsText(file);

    reader.onload = () => {
        tanulok = [];
        letolteshez = [];

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
            letolteshez.push(element);
        });

        console.log(tanulok);

        tanulok.sortBy('Név', 'OM azonosító');
        ChangeLastSort('Név');
        BetoltAdatok();
        document.querySelector('#download').hidden = false;
    };

    reader.onerror = () => console.log(reader.error);
}

let CsvGeneral = () => {
    const res = [Object.keys(letolteshez[0]).join(';')];
    
    tanulok.map(G => G['OM azonosító']).forEach(OM =>{
        res.push(Object.values(letolteshez.find(G => G.OM_Azonosito == OM)).join(';'))
    });
  
    const dataBlob = new Blob([res.join('\n')], { type: 'text/csv' });

    document.querySelector('#download').href = URL.createObjectURL(dataBlob);
};