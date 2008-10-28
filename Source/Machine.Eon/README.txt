Give a set of Assemblies:

Metadata on Assemblies, Namespaces, Types, Methods...

Specifications I would like to write...

No types from Namespaces [A,B,C] should use types in namespaces [D,E,F] (but the inverse is allowed)
Types using Types [A] should only be Types [B,C,D]
Types using Methods [A] should only be Types [B,C,D]
Types using Methods [A] should only be Methods [B,C,D]

Generalized Matchers:
  Namespace N
  Type T
  Property P
  Method M
  Any type
  Any namespace
  Any method
  Any property
  Any setter
  Any getter
  Is System Type
  Using (Nodes)
  
Get nodes using any setter of any property of any type in specific namespace A