
export interface IResourceType {
    resourceTypeId:number,
    description:string
}

export enum ResourceType {
    File = 1,
    Link,
    Form,
    Video,
    AzureProfileImage,
    AzureClientImage,
    AzureClientFile
}