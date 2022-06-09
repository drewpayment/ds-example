/**
 * client group model represents the back-end object
 */
export class ClientGroupDto {
    clientGroupId: number;
    clientId: number;
    code: string;
    description: string;
    hasClientGLAssignment: boolean;
}