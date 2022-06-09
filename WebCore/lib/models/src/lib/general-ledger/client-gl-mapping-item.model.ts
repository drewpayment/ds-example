import { ClientGLAssignment } from './client-gl-assignment.model';

export interface ClientGLMappingItem extends ClientGLAssignment {
    clientGLAssignmentId : number;
    classId: number;
    classCode: number;
    classDescription: number;
    isActive: number;
}