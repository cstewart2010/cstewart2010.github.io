/**
 * @typedef TaxRateData
 * @prop {number} taxRate
 * @prop {number} lowerBound
 * @prop {number?} upperBound
 * @typedef BracketData
 * @prop {string} friendlyName
 * @prop {number} status
 * @prop {number} standardDeduction
 * @prop {TaxRateData[]} rates
 */

const FICA_RATE = .0765;
const INCOME_INPUT_SELECTOR = "#income";
const RETIREMENT_INPUT_SELECTOR = "#retirement";
const HSA_INPUT_SELECTOR = "#hsa";
const PAYROLL_INPUT_SELECTOR = "#payroll";
const INFANT_INPUT_SELECTOR = "#infant";
const YOUNG_INPUT_SELECTOR = "#young";
const ADULT_INPUT_SELECTOR = "#adult";
const TAX_BURDEN_TEXT_SELECTOR = "#tax-burden";
const FICA_BURDEN_TEXT_SELECTOR = "#fica-burden";
const FILING_STATUS_SELECT_SELECTOR = "#filing-status";
const CREDIT_TEXT_SELECTOR = "#credits";
const EFFECTIVE_TAX_RATE_TEXT_SELECTOR = "#effective-tax-rate";

function calculate(){
    const income = Number.parseInt(getValueFromElement(INCOME_INPUT_SELECTOR) || 0);
    const bracket = getBracketData();
    if (!bracket){
        return;
    }

    const deductions = getTaxDeductions(bracket);
    const credits = getTaxCredits();
    const taxableIncome = income - deductions;
    const taxBurden = Math.ceil(getTaxBurden(bracket, taxableIncome));
    const ficaBurden = Math.ceil(income * FICA_RATE);
    const effectiveTaxRate = getEffectiveTaxRate(income, taxBurden);
    setElementText(TAX_BURDEN_TEXT_SELECTOR, `$${taxBurden}`);
    setElementText(FICA_BURDEN_TEXT_SELECTOR, `$${ficaBurden}`);
    setElementText(CREDIT_TEXT_SELECTOR, `$${credits}`);
    setElementText(EFFECTIVE_TAX_RATE_TEXT_SELECTOR, `${effectiveTaxRate}%`);
}

/**
 * 
 * @param {string} selector 
 * @returns {string}
 */
const getValueFromElement = (selector) => {
    return document.querySelector(selector).value;
}

/**
 * 
 * @param {string} selector 
 * @param {string} text 
 */
const setElementText = (selector, text) => {
    document.querySelector(selector).innerHTML = text;
}

/**
 * 
 * @returns {BracketData}
 */
const getBracketData = () => {
    const inputElement = document.querySelector(FILING_STATUS_SELECT_SELECTOR);
    const index = getTransformedIntInput(
        inputElement,
        input => input > 0 && input <= brackets.length,
        input => input - 1
    );

    if (index >= 0){
        return brackets[index]
    }

    return null;
}

/**
 * 
 * @param {BracketData} bracket 
 */
const getTaxDeductions = (bracket) => {
    const retirementInputElement = document.querySelector(RETIREMENT_INPUT_SELECTOR);
    const hsaInputElement = document.querySelector(HSA_INPUT_SELECTOR);
    const payrollDeductionsInputElement = document.querySelector(PAYROLL_INPUT_SELECTOR);
    let totalDeductions = bracket.standardDeduction;
    totalDeductions += getTransformedIntInput(
        retirementInputElement,
        input => input > 0 && input <= 23000,
        input => input
    );
    totalDeductions += getTransformedIntInput(
        hsaInputElement,
        input => input > 0 && input <= 8300,
        input => input
    );
    totalDeductions += getTransformedIntInput(
        payrollDeductionsInputElement,
        input => input > 0,
        input => input * 12
    );

    return totalDeductions;
}

const getTaxCredits = () => {
    const infantDependentsInputElement = document.querySelector(INFANT_INPUT_SELECTOR);
    const youngDependentsInputElement = document.querySelector(YOUNG_INPUT_SELECTOR);
    const adultDependentsInputElement = document.querySelector(ADULT_INPUT_SELECTOR);
    let totalCredits = 0;
    totalCredits += getTransformedIntInput(
        infantDependentsInputElement,
        input => input > 0,
        input => input * 3600
    );
    totalCredits += getTransformedIntInput(
        youngDependentsInputElement,
        input => input > 0,
        input => input * 3000
    );
    totalCredits += getTransformedIntInput(
        adultDependentsInputElement,
        input => input > 0,
        input => input * 500
    );

    return totalCredits;
}

/**
 * 
 * @param {BracketData} bracket 
 * @param {number?} taxableIncome 
 */
const getTaxBurden = (bracket, taxableIncome) => {
    const actualTaxableIncome = taxableIncome > 0 ? taxableIncome : 0;
    let taxBurden = 0;
    if (actualTaxableIncome == 0)
    {
        return taxBurden;
    }
    for (const rate of bracket.rates){
        if (!rate.upperBound || rate.upperBound > taxableIncome){
            taxBurden += rate.taxRate * (taxableIncome - rate.lowerBound);
            break;
        }

        taxBurden += rate.taxRate * (rate.upperBound - rate.lowerBound);
    }

    return taxBurden;
}

/**
 * 
 * @param {number} income 
 * @param {number} taxBurden 
 * @returns 
 */
const getEffectiveTaxRate = (income, taxBurden) => {
    if (income === 0){
        return 0;
    }
    return taxBurden / income * 100;
}

/**
 * 
 * @param {HTMLInputElement} inputElement 
 * @param {(input: number) => boolean} validator 
 * @param {(input: number) => number} transformation 
 */
const getTransformedIntInput = (
    inputElement,
    validator,
    transformation
) => {
    const input = Number.parseInt(inputElement.value);
    if (validator(input)){
        return transformation(input)
    }

    return 0;
}