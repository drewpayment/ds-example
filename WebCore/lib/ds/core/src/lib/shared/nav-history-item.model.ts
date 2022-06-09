

export interface NavHistoryItem {
    source: number;
    dest: number;
    parent: number;
    childNodes?: NavHistoryItem[];
}

