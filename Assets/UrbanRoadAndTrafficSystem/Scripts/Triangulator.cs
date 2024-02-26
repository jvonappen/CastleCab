using System.Collections.Generic;
using UnityEngine;

namespace URNTS
{
    public static class Triangulator
    {
        public static int[] Triangulate(Vector3[] vertices, int startIndex)
        {
            List<int> indices = new List<int>();

            int n = vertices.Length;
            if (n < 3)
                return indices.ToArray();

            int[] V = new int[n];
            if (Area(vertices) > 0)
            {
                for (int v = 0; v < n; v++)
                    V[v] = v;
            }
            else
            {
                for (int v = 0; v < n; v++)
                    V[v] = n - 1 - v;
            }

            int nv = n;
            int count = 2 * nv;
            for (int m = 0, v = nv - 1; nv > 2;)
            {
                if (count-- <= 0)
                    return indices.ToArray();

                int u = v;
                if (nv <= u)
                    u = 0;
                v = u + 1;
                if (nv <= v)
                    v = 0;
                int w = v + 1;
                if (nv <= w)
                    w = 0;

                if (Snip(vertices, u, v, w, nv, V))
                {
                    int a, b, c, s, t;

                    a = V[u];
                    b = V[v];
                    c = V[w];
                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(c);

                    m++;
                    for (s = v, t = v + 1; t < nv; s++, t++)
                        V[s] = V[t];
                    nv--;

                    count = 2 * nv;
                }
            }

            indices.Reverse();
            for (int i = 0; i < indices.Count; i++)
            {
                indices[i] += startIndex;
            }
            return indices.ToArray();
        }

        private static float Area(Vector3[] vertices)
        {
            int n = vertices.Length;
            float A = 0;
            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Vector3 pval = vertices[p];
                Vector3 qval = vertices[q];
                A += pval.x * qval.z - qval.x * pval.z;
            }
            return A * 0.5f;
        }

        private static bool Snip(Vector3[] vertices, int u, int v, int w, int n, int[] V)
        {
            int p;
            Vector3 A = vertices[V[u]];
            Vector3 B = vertices[V[v]];
            Vector3 C = vertices[V[w]];

            if (Mathf.Epsilon > (B.x - A.x) * (C.z - A.z) - (B.z - A.z) * (C.x - A.x))
                return false;

            for (p = 0; p < n; p++)
            {
                if (p == u || p == v || p == w)
                    continue;
                Vector3 P = vertices[V[p]];
                if (InsideTriangle(A, B, C, P))
                    return false;
            }
            return true;
        }

        private static bool InsideTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
            float cCROSSap, bCROSScp, aCROSSbp;

            ax = C.x - B.x;
            ay = C.z - B.z;
            bx = A.x - C.x;
            by = A.z - C.z;
            cx = B.x - A.x;
            cy = B.z - A.z;
            apx = P.x - A.x;
            apy = P.z - A.z;
            bpx = P.x - B.x;
            bpy = P.z - B.z;
            cpx = P.x - C.x;
            cpy = P.z - C.z;

            aCROSSbp = ax * bpy - ay * bpx;
            cCROSSap = cx * apy - cy * apx;
            bCROSScp = bx * cpy - by * cpx;

            return aCROSSbp >= 0 && bCROSScp >= 0 && cCROSSap >= 0;
        }

        public static int[] QuadTriangles(int a, int b, int c, int d, int startIndex = 0)
        {
            return new int[] { startIndex + a, startIndex + b, startIndex + c, startIndex + a, startIndex + c, startIndex + d };
        }

        public static int[] StripTriangulate(Vector3[] rail1, Vector3[] rail2, int startIndex, bool opposite = false)
        {
            if (rail1.Length != rail2.Length)
            {
                Debug.LogError("Length of rails must be equal");
            }
            int l = rail1.Length;
            List<int> list = new List<int>();
            for (int i = 0; i < rail1.Length - 1; i++)
            {
                if (opposite)
                {
                    list.AddRange(QuadTriangles(startIndex + i, startIndex + i + l, startIndex + i + l + 1, startIndex + i + 1));

                }
                else
                {
                    list.AddRange(QuadTriangles(startIndex + i, startIndex + i + 1, startIndex + i + l + 1, startIndex + i + l));
                }
            }
            return list.ToArray();
        }
    }
}