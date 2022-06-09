export class Timezone{
    description: string;
    utcOffset: string;
    descWithUtcOffset: string;
    constructor(description: string, utcOffset: string) {
        this.description = description;
        this.utcOffset = utcOffset;
        this.descWithUtcOffset = `${this.description} (UTC ${this.utcOffset})`;
    }
}