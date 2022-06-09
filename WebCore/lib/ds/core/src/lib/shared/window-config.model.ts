
export interface WindowConfig {
    width?: number;
    height?: number;
    /** INTERNAL ONLY */
    top?: number;
    /** INTERNAL ONLY */
    left?: number;
    status?: string;
    toolbar?: string;
    menubar?: string;
    location?: string; 
    scrollbars?: string;
    resizable?: string;
    fullscreen?: string;
}
