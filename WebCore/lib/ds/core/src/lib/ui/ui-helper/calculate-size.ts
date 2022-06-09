import { coerceNumberProperty } from '@angular/cdk/coercion';

export class CalculateSize {
    static cache = {};
    
    options: CalculateSizeOptions;
    
    static getSizeOfText = (text: string, options?: CalculateSizeOptionalOptions): CalculateSizeSizing => {
        const cacheKey = JSON.stringify({ text: text, options: options });
        
        if (CalculateSize.cache[cacheKey])
            return CalculateSize.cache[cacheKey];
        
        const instance = new CalculateSize(options);
        const size = instance.createSizingElement(text);
        
        CalculateSize.cache[cacheKey] = size;
        return size;
    }
    
    constructor(options: CalculateSizeOptionalOptions) {
        this.options = {
            font: 'Source Sans Pro',
            fontSize: (16 * 0.875).toString(),
            fontWeight: 'normal',
            lineHeight: 'normal',
            width: 'auto',
            wordBreak: 'normal'
        };
        
        if (options) {
            this.options = Object.assign({}, this.options, options) as CalculateSizeOptions;
        }
    }
    
    createSizingElement(text: string): CalculateSizeSizing {
        const e = document.createElement('div');
        const textNode = document.createTextNode(text);
        
        e.appendChild(textNode);
        
        e.style.fontFamily = this.options.font;
        e.style.fontSize = this.options.fontSize;
        e.style.fontWeight = this.options.fontWeight;
        e.style.lineHeight = this.options.lineHeight;
        e.style.position = 'absolute';
        e.style.visibility = 'hidden';
        e.style.left = '-999px';
        e.style.top = '-999px';
        e.style.width = this.options.width;
        e.style.height = 'auto';
        e.style.wordBreak = this.options.wordBreak;
        
        // get computed style
        // const style = window.getComputedStyle(e);
        // don't think we need to add this to the dom anymore...
        document.body.appendChild(e);
        
        const result = {
            width: coerceNumberProperty(e.offsetWidth),
            height: coerceNumberProperty(e.offsetHeight)
        };
        // don't think we need to add this to the dom anymore...
        this.destroyElement(e);
        return result;
    }
    
    destroyElement(element: HTMLElement): void {
        if (!element) return;
        element.parentNode.removeChild(element);
    }
    
}

export interface CalculateSizeSizing {
    width: number;
    height: number;
}

export interface CalculateSizeOptionalOptions {
    font?: string;
    fontSize?: string;
    fontWeight?: string;
    lineHeight?: string;
    width?: string;
    wordBreak?: string;
  }

interface CalculateSizeOptions {
    font: string;
    fontSize: string;
    fontWeight: string;
    lineHeight: string;
    width: string;
    wordBreak: string;
}
