public struct XXHash
{
	const uint primeA = 0b10011110001101110111100110110001;
	const uint primeB = 0b10000101111010111100101001110111;
	const uint primeC = 0b11000010101100101010111000111101;
	const uint primeD = 0b00100111110101001110101100101111;
	const uint primeE = 0b00010110010101100110011110110001;

	readonly uint accumulator;

	public XXHash(uint accumulator)
	{
		this.accumulator = accumulator;
	}

	public static implicit operator XXHash(uint accumulator) =>
		new XXHash(accumulator);

	public static XXHash Seed(int seed) => (uint)seed + primeE;

	static uint RotateLeft(uint data, int steps) =>
		(data << steps) | (data >> 32 - steps);

	public XXHash Eat(int data) =>
		RotateLeft(accumulator + (uint)data * primeC, 17) * primeD;

	public XXHash Eat(byte data) =>
		RotateLeft(accumulator + data * primeE, 11) * primeA;

	public static implicit operator uint(XXHash hash)
	{
		uint avalanche = hash.accumulator;
		avalanche ^= avalanche >> 15;
		avalanche *= primeB;
		avalanche ^= avalanche >> 13;
		avalanche *= primeC;
		avalanche ^= avalanche >> 16;
		return avalanche;
	}
}
