using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Numerics;

[RequireComponent(typeof(MeshRenderer))]
public class ScreenManager : MonoBehaviour
{   
    public List<Complex> roots;

    public int n = 10;

    public int width = 200;
    public int height = 200;

    public double divisor = 1;

    private Texture2D pixels;

    public int f_count;
    public int f_diff = 30;

    public Color[] colours;

    public bool update = false;
    void Start()
    {
        roots = new List<Complex>();
        //The roots of our polynomial.
        roots.Add(new Complex(-1.3247,  0      ));
        roots.Add(new Complex(      0,  1      ));
        roots.Add(new Complex(      0, -1      ));
        roots.Add(new Complex(0.66236, -0.56228));
        roots.Add(new Complex(0.66236,  0.56228));

        colours = new Color[] {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.magenta           
        };
        f_count = 0;

        updateTexture();
    }

    void Update()
    {
        if (f_count % f_diff == 0 && update == true) {
            updateTexture();
        }
        f_count++;
    }
    
    [ContextMenu("Update Texture")]
    public void updateTexture() {
        //create the renderTexture
            pixels = new Texture2D(width, height);

            Material m = GetComponent<MeshRenderer>().material;

            m.mainTexture = pixels;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Color color = getColour(x,y);
                    pixels.SetPixel(x, y, color);
                }
            }

            pixels.Apply();

            GetComponent<MeshRenderer>().material = m;
    }

    private Color getColour(int x, int y) {
        double newx = (x - width/2) / divisor;
        double newy = (y - height/2) / divisor;
        return colours[indexOfClosest(iterate(new Complex(newx, newy), n))];
    }

    private int indexOfClosest(Complex c) {
        double dist = Complex.Abs(c - roots[0]);
        int index = 0;
        for (int i = 1; i < roots.Count; i++) {
            double newdist = Complex.Abs(c - roots[i]);

            if (newdist < dist) {
                dist = newdist;
                index = i;
            }
        }

        return index;
    }

    private Complex iterate(Complex c, int n) {
        if (n == 0) {
            return c;
        } else {
            Complex d = iterate(c, n-1);
            return d - ( P(d) / Pdash(d) );
        }
    }



    /*
    P(z) = z^5 + z^2 -z +1
    z = -1.3247
    z = i
    z = -i
    z = 0.66236 - 0.56228i
    z = 0.66236 + 0.56228i
     */    
    private Complex P(Complex z) {
        return Complex.Pow(z, 5) + Complex.Pow(z, 2) - z + 1;
    }

    private Complex Pdash(Complex z) {
        return 5*Complex.Pow(z, 4) + 2*z - 1;
    }
}
