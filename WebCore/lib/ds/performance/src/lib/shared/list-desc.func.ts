export function listDesc(str){
    var br = String.fromCharCode(10);
    if(str.indexOf(br)>-1){
        return str.replace(/\n/gi, "<br/>");
    }
    return str;
  }