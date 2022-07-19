using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

public class HashVisualisation : MonoBehaviour
{

	[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
	struct HashJob : IJobFor
	{

		[WriteOnly]
		public NativeArray<uint> hashes;

		public void Execute(int i)
		{
			hashes[i] = (uint)i;
		}
	}
}
