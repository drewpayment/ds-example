
export interface SidebarConfiguration {
    hasPerformance: boolean;
    hasGoalTracking: boolean;
    sidebarType: SidebarMenuType;
    
}

export enum SidebarMenuType {
    default,
    performance
}
