using System.Collections.Generic;
using UnityEngine;

public class GlGizmo : MonoBehaviour
{
    [SerializeField][Range(3,32)] int _lineCount = 12;
    [SerializeField][Range(0.001f,100f)] float _radius = 1.5f;
    [SerializeField][Range(0.001f,1f)] float _unitWidth = 0.1f;//width when distance to camera is 1.0

    void OnDrawGizmos ()
    {
        System.Func<int,int,Vector3> CalcPoint = ( i , max ) =>
        {
            float f = i / (float)max;
            float angle = f * Mathf.PI * 2;
            return new Vector3( Mathf.Cos(angle)*_radius , Mathf.Sin(angle)*_radius );
        };

        float dist = Vector3.Distance( Camera.current.transform.position , transform.position );

        GL.PushMatrix();
        GL.MultMatrix( transform.localToWorldMatrix );
        ( new Material( Shader.Find("Sprites/Default") ) ).SetPass(0);//replace material to customize render style
        GL.Begin( GL.QUADS );
        {
            //set color:
            GL.Color( Color.white );

            //warm up:
            {
                Vector3 A0 = CalcPoint( 0 , _lineCount );
                Vector3 B0 = A0 + A0.normalized*_unitWidth*dist;
                GL.Vertex( A0 );
                GL.Vertex( B0 );
            }

            //segments:
            for( int i=1 ; i<=_lineCount ; i++ )
            {
                Vector3 A = CalcPoint( i , _lineCount );
                Vector3 B = A + A.normalized*_unitWidth*dist;
                
                if( i%2==0 )//even
                {
                    //end started quad:
                    GL.Vertex( A );
                    GL.Vertex( B );

                    //start new quad
                    GL.Vertex( A );
                    GL.Vertex( B );
                }
                else//odd
                {
                    //end started quad:
                    GL.Vertex( B );
                    GL.Vertex( A );

                    //start new quad
                    GL.Vertex( B );
                    GL.Vertex( A );
                }
            }
        }
        GL.End();
        GL.PopMatrix();
    }
}
