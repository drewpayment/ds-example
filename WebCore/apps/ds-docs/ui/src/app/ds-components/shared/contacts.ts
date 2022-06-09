import { IContact } from '@ds/core/contacts';

export const DEMO_CONTACTS: IContact[] = [
    { 
        firstName: "John", 
        lastName: "Smith", 
        userId: 100,
        employeeId: 100,
        profileImage: {
            employeeId: null,
            clientGuid: null,
            clientId: null,
            employeeGuid: null,
            sasToken: null,
            profileImageInfo: [],
            extraLarge: {
                hasImage: true,
                url: ""
            }
        }
    },
    { 
        firstName: "Jane", 
        lastName: "Smith", 
        userId: 101,
        employeeId: 101,
        profileImage: {
            employeeId: null,
            clientGuid: null,
            clientId: null,
            employeeGuid: null,
            sasToken: null,
            profileImageInfo: [],
            extraLarge: {
                hasImage: true,
                url: ""
            }
        }
    },
    { 
        firstName: "Mary", 
        lastName: "Clark", 
        userId: 102,
        employeeId: 102,
        profileImage: {
            employeeId: null,
            clientGuid: null,
            clientId: null,
            employeeGuid: null,
            sasToken: null,
            profileImageInfo: [],
            extraLarge: {
                hasImage: true,
                url: "https://randomuser.me/api/portraits/women/55.jpg"
            }
        }
    },
    { 
        firstName: "George", 
        lastName: "Weller", 
        userId: 103,
        employeeId: 103,
        profileImage: {
            employeeId: null,
            clientGuid: null,
            clientId: null,
            employeeGuid: null,
            sasToken: null,
            profileImageInfo: [],
            extraLarge: {
                hasImage: true,
                url: "https://randomuser.me/api/portraits/men/18.jpg"
            }
        }
    },
]