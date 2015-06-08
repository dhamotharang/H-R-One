// JScript File

function getHKIDCheckDigit(hkidWithoutCheckDigit)
{
    hkidWithoutCheckDigit=hkidWithoutCheckDigit.toUpperCase();
    if (hkidWithoutCheckDigit.length>=7 && hkidWithoutCheckDigit.length<=8)
    {
        if (hkidWithoutCheckDigit.length==7)
            hkidWithoutCheckDigit=" " +hkidWithoutCheckDigit;
        var sum=0;
        for(var idx=0;idx<hkidWithoutCheckDigit.length;idx++)
        {
            var digit= hkidWithoutCheckDigit.charCodeAt(idx);
            if (digit>=48 && digit<=57)
                digit=digit-48;
            else if (digit>=65 && digit <=90)
                digit=digit - 55;
            else if (digit==32)
                digit=36;
            sum += digit * (9-idx);
        }
        var checkDigit = 11 - (sum % 11);
        if (checkDigit==11)
            return 0;
        else if (checkDigit ==10)
            return "A";
        else
            return checkDigit;
    }
    return "";
}