// Asigna eventos a los inputs de una fila específica (sin clonar ni reemplazar, solo asigna listeners)
function attachRowEvents(row, idx) {
    const qtyInput = row.querySelector('.qty-input');
    const unitCostInput = row.querySelector('.unitcost-input');
    const costTypeInput = row.querySelector('.costtype-input');
    if (qtyInput) qtyInput.oninput = function () { calcRow(row, idx); updateSalesTaxAmount(); };
    if (unitCostInput) unitCostInput.oninput = function () { calcRow(row, idx); updateSalesTaxAmount(); };
    if (costTypeInput) costTypeInput.onchange = function () { calcRow(row, idx); updateSalesTaxAmount(); };
}

// Calcula y muestra el Sales Tax Amount en tiempo real
function updateSalesTaxAmount() {
    const salesTaxSelect = document.getElementById('SalesTax');
    if (!salesTaxSelect) return;
    const salesTax = parseFloat(salesTaxSelect.value) || 0;
    let total = 0;
    document.querySelectorAll('#itemsTable tbody tr').forEach(function(row) {
        const costType = row.querySelector('.costtype-input')?.value;
        if (costType === 'Material') {
            const totalCostStr = row.querySelector('.totalcost-output')?.value || '0';
            // Quitar $ y convertir a número
            const totalCost = parseFloat(totalCostStr.replace(/[^\d,\.]/g, '').replace(',', '.')) || 0;
            total += totalCost;
        }
    });
    const salesTaxAmount = total * salesTax;
    let formatted = salesTaxAmount.toLocaleString('es-MX', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    formatted = formatted.replace('.', ',');
    const el = document.getElementById('salesTaxAmountValue');
    if (el) el.textContent = `$${formatted}`;
}

// Función global para agregar una fila (usada en ambas vistas)
window.addRow = function() {
    var table = document.getElementById('itemsTable').getElementsByTagName('tbody')[0];
    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);
    var fields = ["CostCode", "Title", "Description", "Qty", "UnitCost", "Margin", "CustomerCost", "TotalCost", "CostType", "MarkupPercentage"];
    for (var i = 0; i < fields.length; i++) {
        var cell = row.insertCell(i);
        if (fields[i] === "CostCode") {
            cell.innerHTML = `<select name="Items[${rowCount}].CostCode" class="form-select">
                <option value="">Selecciona</option>
                <option value="Andersen">Andersen</option>
                <option value="Awake">Awake</option>
                <option value="Trim">Trim</option>
                <option value="Otro">Otro</option>
            </select>`;
        } else if (fields[i] === "CostType") {
            cell.innerHTML = `<select name="Items[${rowCount}].CostType" class="form-select costtype-input">
                <option value="">Selecciona</option>
                <option value="Material">Material</option>
                <option value="Labor">Labor</option>
                <option value="Other">Other</option>
            </select>`;
        } else if (fields[i] === "Margin") {
            cell.innerHTML = `<input class="form-control margin-output" type="text" value="0,00" readonly />`;
        } else if (fields[i] === "CustomerCost") {
            cell.innerHTML = `<input class="form-control customercost-output" type="text" value="0,00" readonly />`;
        } else if (fields[i] === "TotalCost") {
            cell.innerHTML = `<input class="form-control totalcost-output" type="text" value="0,00" readonly />`;
        } else if (fields[i] === "MarkupPercentage") {
            cell.innerHTML = `<input class="form-control markup-output" type="text" value="0,00" readonly />`;
        } else if (fields[i] === "Qty") {
            cell.innerHTML = `<input name="Items[${rowCount}].Qty" class="form-control qty-input" type="number" step="0.01" value="0" />`;
        } else if (fields[i] === "UnitCost") {
            cell.innerHTML = `<input name="Items[${rowCount}].UnitCost" class="form-control unitcost-input" type="number" step="0.01" value="0" />`;
        } else {
            cell.innerHTML = `<input name="Items[${rowCount}].${fields[i]}" class="form-control" />`;
        }
    }
    var cell = row.insertCell(fields.length);
    cell.innerHTML = `<button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">Eliminar</button>`;
    // Reasigna eventos solo a las filas de datos (tbody)
    const rows = table.querySelectorAll('tbody tr');
    rows.forEach((r, idx) => attachRowEvents(r, idx));
    calcRow(row, rowCount);
    updateSalesTaxAmount();
}

// Función global para eliminar una fila (usada en ambas vistas)
window.removeRow = function(btn) {
    var row = btn.parentNode.parentNode;
    row.parentNode.removeChild(row);
    recalcAllRows();
    updateSalesTaxAmount();
}
// Calcula todos los campos automáticos para cada fila de partidas
function recalcAllRows() {
    const table = document.getElementById('itemsTable');
    if (!table) return;
    const rows = table.querySelectorAll('tbody tr');
    rows.forEach((row, idx) => {
        calcRow(row, idx);
    });
}

function calcRow(row, idx) {
    // Busca los inputs por clase y por fila
    const qtyInput = row.querySelector('.qty-input');
    const unitCostInput = row.querySelector('.unitcost-input');
    const marginInput = row.querySelector('.margin-output');
    const customerCostInput = row.querySelector('.customercost-output');
    const totalCostInput = row.querySelector('.totalcost-output');
    const markupInput = row.querySelector('.markup-output');
    const costTypeInput = row.querySelector('.costtype-input');
    if (!qtyInput || !unitCostInput || !marginInput || !customerCostInput || !totalCostInput || !markupInput || !costTypeInput) return;

    const qty = parseFloat(qtyInput.value) || 0;
    const unitCost = parseFloat(unitCostInput.value) || 0;
    const customerCost = unitCost / 0.65;
    const margin = customerCost !== 0 ? 1 - (unitCost / customerCost) : 0;
    let markupPercentage = 0;
    if (margin < 1 && margin > 0) {
        markupPercentage = (margin / (1 - margin)) * 100;
    }
    const totalCost = qty * customerCost;

    // Formatea con coma como separador decimal (es-MX)
    // Forzar idioma y región para navegadores que ignoran 'es-MX'
    const locale = navigator.languages && navigator.languages.includes('es-MX') ? 'es-MX' : (navigator.languages && navigator.languages.includes('es') ? 'es' : 'es-ES');
    marginInput.value = margin.toLocaleString(locale, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    customerCostInput.value = customerCost.toLocaleString(locale, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    totalCostInput.value = totalCost.toLocaleString(locale, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    markupInput.value = markupPercentage.toLocaleString(locale, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    // Depuración: mostrar en consola el valor formateado
    console.log('margin', marginInput.value, 'locale', locale);
}

// Asigna eventos a todos los inputs relevantes de cada fila
document.addEventListener('DOMContentLoaded', function () {
    const table = document.getElementById('itemsTable');
    if (!table) return;
    const rows = table.querySelectorAll('tbody tr');
    rows.forEach((row, idx) => {
        attachRowEvents(row, idx);
        calcRow(row, idx);
    });
    recalcAllRows();
    updateSalesTaxAmount();
    const salesTaxSelect = document.getElementById('SalesTax');
    if (salesTaxSelect) {
        salesTaxSelect.addEventListener('change', updateSalesTaxAmount);
    }
});
