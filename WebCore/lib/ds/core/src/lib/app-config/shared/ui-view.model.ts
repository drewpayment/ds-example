

export interface UIViewState {
    data: UIViewData;
    name: string;
    parent: string;
    url: string;
    view: UIViewTemplates;
}

export interface UIViewData {
    menuName: string;
    stylesheet: string;
    title: string;
}

export interface UIViewTemplates {
    [key: string]: string;
}
