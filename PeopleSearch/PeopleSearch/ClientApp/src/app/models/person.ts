import { Address } from './address'

export interface Person {
    PersonId: number;
    FirstName: string;
    MiddleName: string;
    LastName: string;
    ProfileImage: string;
    DateOfBirthUtc: string;
    Address: Address;
    Interests: string[];
}