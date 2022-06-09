/**
 * Takes variable, finds the TypeScript type, and converts to the appropriate Backend enum value, corresponding to C# types.
 * @param itemToType variable to find the type of and output as enum
 * @returns string containing an integer, that corresponds to the enum located at Ds.Source/DataServices/dsEffectiveDate.vb
*/



export function convertToBackendTypeEnum(itemToType: any){
    if(typeof itemToType == "boolean"){ //check if type is boolean, return 6 if true
        return "6";
    }
    else if(Number(itemToType) != NaN){ //check to see if the item is a number
        if(Number(itemToType) % 1 != 0){ //if it is, use mod to see if its an int or float(double)
            return "2";
        }
        else{
            return "4";
        }
    }
    else if(typeof itemToType == "string"){ //else check the type to see if its a string, return 1
        return "1";
    }
}
